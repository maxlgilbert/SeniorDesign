
%compile clpmex
% path_coin_clp = '/home/dinesh/Desktop/coin-Clp';
% mex -v clpmex.cpp -I/home/dinesh/Desktop/coin-Clp/include/coin -I/home/dinesh/Desktop/coin-Clp/CoinUtils/src -I/home/dinesh/Desktop/coin-Clp/Clp/src -L/home/dinesh/Desktop/coin-Clp/lib -lClp -lCoinUtils -largeArrayDims -lut -lbz2


% compile mexclp
% cd mexclp
% mex mexclp.cpp -I/home/dinesh/Desktop/coin-Clp/include/coin -L/home/dinesh/Desktop/coin-Clp/lib -lClp -lCoinUtils -lbz2



function test_supreg()
 addpath('./mexclp')
 t = load('sample1.mat')
 [nvert, Yin, nlines, lines] = supreg(t.n,t.ri,t.nu,t.mus,t.M,t.epsilon)

end




function [nvert, Yin, nlines, lines] = supreg(n,r,nu,mus,M,epsilon)

nvert= 0;
  Yin=0;
  nlines= 0;
  lines= 0;
  
% supreg obtains an approximation to the support region of a body. 
%Inputs:
    % n       - Number of contact poins with a surface. Scalar. 
    % r       - Matrix of positions of the contact points with a surface. 3xn,  
              % column i is the position vector of contact point i. 
    % nu      - Matrix of vectors normal to the surface of contact at each
              % contact point. 3xn, column i is the normal vector associated 
              % with contact point i.
    % mus     - Coefficient of static friction. Scalar.   
    % M       - Total mass. Scalar. 
    % epsilon - Maximum acceptable difference between the inner and outer 
              % approximations of the support region. Scalar.
%Outputs: 
    % nvert   - Number of vertices defining the approximation of the support
              % region. Scalar.
    % Yin     - Matrix with the [x;y] coordinates of the vertices defining the
              % approximation of the support region. 2xnvert.
    % nlines  - Number of lines that define the half planes whose 
              %intersection defines the approximation to the support region. 
    % lines   - Lines a*x + b*y = c that define the half planes whose 
              % intersection defines the approximation to the support region.
              % 3xnlines, where each column has the form [a;b;c].

    if n==0
        disp('No support region!!!! X_x');
        nvert = [];
        Yin = [];
        nlines = [];
        lines = [];
        return
    end
    
   
    %Get Q's

    for i=1:1:n
        Q(i).Q = [ nu(1,i) , nu(2,i)-nu(3,i) , nu(2,i)*(nu(1,i)-nu(2,i))-nu(3,i)*(nu(3,i)-nu(1,i)) ;...
                   nu(2,i) , nu(3,i)-nu(1,i) , nu(3,i)*(nu(2,i)-nu(3,i))-nu(1,i)*(nu(1,i)-nu(2,i)) ;...
                   nu(3,i) , nu(1,i)-nu(2,i) , nu(1,i)*(nu(3,i)-nu(1,i))-nu(2,i)*(nu(2,i)-nu(3,i)) ];
        Q(i).Q(:,2) = Q(i).Q(:,2)/norm( Q(i).Q(:,2) );
        Q(i).Q(:,3) = Q(i).Q(:,3)/norm( Q(i).Q(:,3) );
        R(:,i) = [ 1 ;...
                   0 ;...
                   0 ];
    end
    
    %The friction cone

    for i=1:1:n
        mu(i) = mus;
    end

    for i=1:1:n
        W(i).W(:,:) = [-mu(i) , -1 ,  0 ;...
                       -mu(i) ,  1 ,  0 ;...
                       -mu(i) ,  0 , -1 ;...
                       -mu(i) ,  0 ,  1 ];
    end
    

    g = 9.81*[0;0;-1];
    P = [1 0 0; 0 1 0];

    % A1, A2, t, B1, B2, u

    A1 = zeros(4*n, 3*n);
    for i=1:1:n
        A1( 4*i-3 : 4*i, 3*i-2 : 3*i ) = W(i).W(:,:)*transpose(Q(i).Q(:,:));
    end

    A2 = zeros(4*n,2);

    t = zeros(4*n,1);

    for k=1:1:n
        B1(:,3*k-2 : 3*k) = [     1   ,     0   ,     0   ;...
                                  0   ,     1   ,     0   ;...
                                  0   ,     0   ,     1   ;...
                                  0   , -r(3,k) ,  r(2,k) ;...
                               r(3,k) ,     0   , -r(1,k) ;...
                              -r(2,k) ,  r(1,k) ,     0   ];
    end

    Tmg = M*[    0   , -g(3,1) ,  g(2,1) ;...
                 g(3,1) ,     0   , -g(1,1) ;...
                -g(2,1) ,  g(1,1) ,     0   ];  
    B2 = zeros(6,2);
    B2(4:6,:) =  -Tmg*transpose(P);

    u = zeros(6,1);
    u(1:3,1) =-M*g;
    
    iY = 0;

    Yin = [];
    Youta = [];
    Youtb = [];


    %get three vertices 

    A = [A1, A2];
    B = [B1 B2];
    f = zeros(3*n+2,1);

    a = [1;1];
    a = a/norm(a);
    
    f(3*n+1:3*n+2,1) = a;
    sol = linprog(-f,A,t,B,u);

    zopt = sol(3*n+1:3*n+2,1);


    %---------- Test Opti_clp mex file --------------
    H = [];
    lb = [];
    ub = [];
    
    % Merge constraints
    A1 = [B;A];
    ru = [u;t];
    
    COIN_DBL_MAX = 1000000; %max of double
    rl = -COIN_DBL_MAX*ones(size(ru));
    
    rl(1:size(u,1)) = ru(1:size(u,1));
    
    opts = [];
    opts.display = 1;
    opts.maxiter = 30000;
    opts.algorithm = 'automatic';
    opts.numThreads = 4;
    opts.maxtime = 3000;
    opts.warnings = 'all';
    opts.tolrfun = 1.0000e-07;
    
    [x,fval,exitflag,info] = opti_clp(H,-f,A1,rl,ru,lb,ub,opts);
    
    zopt2 = x(3*n+1:3*n+2,1);
    
    
   % ---------------test mex_clp file--------------
   
   c = -f;
   b = t;
   Aeq = B;
   beq = u;
   lb = [];
   ub = [];
   
   [x,lambda,status] = clp2([],c,A,b,Aeq,beq,lb,ub);
   
    zopt3 = x(3*n+1:3*n+2,1);
    
    disp('comparing outputs of 3 optimizations')
    [zopt zopt2 zopt3]

end
