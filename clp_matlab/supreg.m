function [nvert, Yin, nlines, lines] = supreg(n,r,nu,mus,M,epsilon)


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

    iY = iY+1 ;
    Yin(:,iY) = zopt;
    Youta(:,iY) = a;
    Youtb(iY) = transpose(a)*zopt;
    
    a = [-1;1];
    a = a/norm(a);
    
    f(3*n+1:3*n+2,1) = a;
    sol = linprog(-f,A,t,B,u);

    zopt = sol(3*n+1:3*n+2,1);

    iY = iY+1;
    Yin(:,iY) = zopt;
    Youta(:,iY) = a;
    Youtb(iY) = transpose(a)*zopt;
    
    a = [0;-1];
    a = a/norm(a);
    
    f(3*n+1:3*n+2,1) = a;
    sol = linprog(-f,A,t,B,u);

    zopt = sol(3*n+1:3*n+2,1);

    iY = iY+1;
    Yin(:,iY) = zopt;
    Youta(:,iY) = a;
    Youtb(iY) = transpose(a)*zopt;
 
    for i=1:1:iY
        ang(1,i) = atan2(Yin(2,i),Yin(1,i));
    end
    [~,I] = sort(ang(1,:));
    copyYin   = Yin;
    copyYouta = Youta;
    copyYoutb = Youtb;
    for i=1:1:iY
        Yin(:,i)   = copyYin(:,I(1,i));
        Youta(:,i) = copyYouta(:,I(1,i));
        Youtb(i)   = copyYoutb(I(1,i));
    end

    deltaA = 0;
    
    for i=1:1:(iY-1)
        Yina(:,i) = [(Yin(2,i+1)-Yin(2,i)),-((Yin(1,i+1)-Yin(1,i)))];
        Yina(:,i) = Yina(:,i)/norm(Yina(:,i));
        
        Yout(:,i) =  (1/( Youta(1,i)*Youta(2,i+1)-Youta(2,i)*Youta(1,i+1) ))*[Youta(2,i+1),-Youta(2,i);-Youta(1,i+1),Youta(1,i)]*[Youtb(i);Youtb(i+1)];
        Aout(i) = polyarea([Yin(1,i:i+1),Yout(1,i)],[Yin(2,i:i+1),Yout(2,i)]);
    end
    Yina(:,iY) = [(Yin(2,1)-Yin(2,iY)),-((Yin(1,1)-Yin(1,iY)))]; 
    Yina(:,iY) = Yina(:,iY)/norm(Yina(:,iY));
    
    Yout(:,iY) =  (1/( Youta(1,iY)*Youta(2,1)-Youta(2,iY)*Youta(1,1) ))*[Youta(2,1),-Youta(2,iY);-Youta(1,1),Youta(1,iY)]*[Youtb(iY);Youtb(1)];
    Aout(iY) = polyarea([Yin(1,iY),Yin(1,1),Yout(1,iY)],[Yin(2,iY),Yin(2,1),Yout(2,iY)]);


    deltaA = sum(Aout);
   
  
    while deltaA>epsilon
        
        [~,i] = max(Aout);
        a = Yina(:,i);
        f(3*n+1:3*n+2,1) = a;
        sol = linprog(-f,A,t,B,u);
        
        zopt = sol(3*n+1:3*n+2,1);
        
        if i<iY
            Yin(:,i+2:iY+1) = Yin(:,i+1:iY); 
            Yina(:,i+2:iY+1) = Yina(:,i+1:iY); 
            
            Yout(:,i+2:iY+1) = Yout(:,i+1:iY); 
            Youta(:,i+2:iY+1) = Youta(:,i+1:iY); 
            Youtb(i+2:iY+1) = Youtb(i+1:iY); 
        end
        
        Yin(:,i+1) = zopt;
        Youta(:,i+1) = a;
        Youtb(i+1) = transpose(a)*zopt;
        
        iY = iY+1; 
      
       
        deltaA = 0;
        
         Yina(:,i) = [(Yin(2,i+1)-Yin(2,i)),-((Yin(1,i+1)-Yin(1,i)))];   
         Yina(:,i) = Yina(:,i)/norm(Yina(:,i));

         Yout(:,i) =  (1/( Youta(1,i)*Youta(2,i+1)-Youta(2,i)*Youta(1,i+1) ))*[Youta(2,i+1),-Youta(2,i);-Youta(1,i+1),Youta(1,i)]*[Youtb(i);Youtb(i+1)];

         i = i+1; 

         if i+1>iY
             Yina(:,iY) = [(Yin(2,1)-Yin(2,iY)),-((Yin(1,1)-Yin(1,iY)))];   
             Yina(:,iY) = Yina(:,iY)/norm(Yina(:,iY));

             Yout(:,iY) =  (1/( Youta(1,iY)*Youta(2,1)-Youta(2,iY)*Youta(1,1) ))*[Youta(2,1),-Youta(2,iY);-Youta(1,1),Youta(1,iY)]*[Youtb(iY);Youtb(1)];
         else
             Yina(:,i) = [(Yin(2,i+1)-Yin(2,i)),-((Yin(1,i+1)-Yin(1,i)))];   
             Yina(:,i) = Yina(:,i)/norm(Yina(:,i));

             Yout(:,i) =  (1/( Youta(1,i)*Youta(2,i+1)-Youta(2,i)*Youta(1,i+1) ))*[Youta(2,i+1),-Youta(2,i);-Youta(1,i+1),Youta(1,i)]*[Youtb(i);Youtb(i+1)];
         end
         
         for i=1:1:iY-1
             Aout(i) = polyarea([Yin(1,i:i+1),Yout(1,i)],[Yin(2,i:i+1),Yout(2,i)]);
         end
         Aout(iY) = polyarea([Yin(1,iY),Yin(1,1),Yout(1,iY)],[Yin(2,iY),Yin(2,1),Yout(2,iY)]);
         
        deltaA = sum(Aout);

    end
        
    %fill(Yin(1,:),Yin(2,:),[0 0 1]);
    [~,nvert] = size(Yin);
    
    mimat = Yina;
    mimat(3,:) = transpose( diag( transpose( transpose(Yina)*Yin ) ) );
    mimat = round(mimat*10000)/10000; %Adjust as desired!  
    [~,col]=size(mimat);
    mimat(4:col,:)= ones(col-3,col);
    [~,jb] = rref(mimat);
    [~,col]=size(jb);
    nlines = col;
        
    for i=1:1:col
        lines(1:3,i)=mimat(1:3,jb(1,i));  
    end

    
end
