//
//  supreg.cpp
//  AtlasResearchC++
//
//  Created by Max Gilbert on 9/19/14.
//  Copyright (c) 2014 UPenn. All rights reserved.
//

#include "Supreg.h"
#include <vector>

bool Supreg::SupportRegion(int n, const Eigen::MatrixXf &r, const Eigen::MatrixXf &nu,
                           float mus, float M, float epsilon, float &nvert, Eigen::MatrixXf &Yin,
                           int &nlines, Eigen::MatrixXf &lines)
{
    
//    if (n==0){
//        std::cout << "No support region!!!! X_x";
//        return false;
//    }
//    
//    // Get Q's
//   /* for i=1:1:n
//        Q(i).Q = [ nu(1,i) , nu(2,i)-nu(3,i) , nu(2,i)*(nu(1,i)-nu(2,i))-nu(3,i)*(nu(3,i)-nu(1,i)) ;...
//                  nu(2,i) , nu(3,i)-nu(1,i) , nu(3,i)*(nu(2,i)-nu(3,i))-nu(1,i)*(nu(1,i)-nu(2,i)) ;...
//                  nu(3,i) , nu(1,i)-nu(2,i) , nu(1,i)*(nu(3,i)-nu(1,i))-nu(2,i)*(nu(2,i)-nu(3,i)) ];
//    Q(i).Q(:,2) = Q(i).Q(:,2)/norm( Q(i).Q(:,2) );
//    Q(i).Q(:,3) = Q(i).Q(:,3)/norm( Q(i).Q(:,3) );
//    R(:,i) = [ 1 ;...
//              0 ;...
//              0 ];
//    end*/
//    std::vector<Eigen::Matrix<float, 3, 3>> QArray = std::vector<Eigen::Matrix<float, 3, 3>>();
//    Eigen::MatrixXf R(3,n);
//    for (int i =0; i < n; i++) {
//        Eigen::Matrix<float, 3, 3> Q = Eigen::Matrix<float,3,3>();
//        Q(0,0) = nu(0,i); Q(0,1) = nu(1,i)-nu(2,i); Q(0,2) = nu(1,i)*(nu(0,i)-nu(1,i))-nu(2,i)*(nu(2,i)-nu(0,i));
//        Q(1,0) = nu(1,i); Q(1,1) = nu(2,i)-nu(0,i); Q(1,2) = nu(2,i)*(nu(1,i)-nu(2,i))-nu(0,i)*(nu(0,i)-nu(1,i));
//        Q(2,0) = nu(2,i); Q(2,1) = nu(0,i)-nu(1,i); Q(2,2) = nu(0,i)*(nu(2,i)-nu(0,i))-nu(1,i)*(nu(1,i)-nu(2,i));
//        Eigen::Vector3f colVec = Eigen::Vector3f(Q(0,1),Q(1,1),Q(2,1));
//        float length1 = colVec.norm();
//        colVec(0) = Q(0,2); colVec(1) = Q(1,2); colVec(2) = Q(2,2);
//        float length2 = colVec.norm();
//        for (int j = 0; j < 3; j++) {
//            Q(j,1) = Q(j,1)/length1;
//            Q(j,2) = Q(j,2)/length2;
//        }
//        QArray.push_back(Q);
//        R(0,i) = 1.0f;
//    }
//    /*for (int i = 0; i < n; i++) {
//        std::cout<< QArray[i] << std::endl;
//        std::cout<<std::endl;
//    }*/
//    
//    //The friction cone
//    /*
//    for i=1:1:n
//        mu(i) = mus;
//    end
//    
//    for i=1:1:n
//        W(i).W(:,:) = [-mu(i) , -1 ,  0 ;...
//                       -mu(i) ,  1 ,  0 ;...
//                       -mu(i) ,  0 , -1 ;...
//                       -mu(i) ,  0 ,  1 ];
//    end*/
//    Eigen::VectorXf mu(n);
//    for (int i = 0; i < n; i++) {
//        mu(i) = mus;
//    }
//    
//    Eigen::Matrix<float, 4, 3> *WArray = new Eigen::Matrix<float, 4, 3>[n]; //TODO FREE!!!
//    for (int i = 0; i < n; i++) {
//        Eigen::Matrix<float, 4, 3> W = Eigen::Matrix<float,4,3>();
//        W(0,0) = -mu(i); W(0,1) = -1; W(0,2) = 0;
//        W(1,0) = -mu(i); W(1,1) = 1; W(1,2) = 0;
//        W(2,0) = -mu(i); W(2,1) = 0; W(2,2) = -1;
//        W(3,0) = -mu(i); W(3,1) = 0; W(3,2) = 1;
//        WArray[i] = W;
//    }
//    
//    
//    /*for (int i = 0; i < n; i++) {
//        std::cout<< WArray[i] << std::endl;
//        std::cout<<std::endl;
//     }*/
//    
//    // A1, A2, t, B1, B2, u
//    /*
//    g = 9.81*[0;0;-1];
//    P = [1 0 0; 0 1 0];
//    
//    % A1, A2, t, B1, B2, u
//    
//    A1 = zeros(4*n, 3*n);
//    for i=1:1:n
//        A1( 4*i-3 : 4*i, 3*i-2 : 3*i ) = W(i).W(:,:)*transpose(Q(i).Q(:,:));
//    end
//    
//    A2 = zeros(4*n,2);
//    
//    t = zeros(4*n,1);
//    
//    for k=1:1:n
//        B1(:,3*k-2 : 3*k) = [     1   ,     0   ,     0   ;...
//                             0   ,     1   ,     0   ;...
//                             0   ,     0   ,     1   ;...
//                             0   , -r(3,k) ,  r(2,k) ;...
//                             r(3,k) ,     0   , -r(1,k) ;...
//                             -r(2,k) ,  r(1,k) ,     0   ];
//    end
//    
//    Tmg = M*[    0   , -g(3,1) ,  g(2,1) ;...
//             g(3,1) ,     0   , -g(1,1) ;...
//             -g(2,1) ,  g(1,1) ,     0   ];
//    B2 = zeros(6,2);
//    B2(4:6,:) =  -Tmg*transpose(P);
//    
//    u = zeros(6,1);
//    u(1:3,1) =-M*g;
//     */
//    Eigen::Vector3f g(0.0f, 0.0f, -9.81f);
//    Eigen::Matrix<float, 2, 3> P;
//    P << 1, 0, 0,
//         0, 1, 0;
//    
//    Eigen::MatrixXf A1(4*n, 3*n);
//    for (int i = 0; i < n; i++) {
//        Eigen::Matrix<float, 4, 3> WQ = WArray[i] * QArray[i].transpose();
//        A1(4*i,3*i)=WQ(0,0); A1(4*i,3*i+1)=WQ(0,1); A1(4*i,3*i+2) = WQ(0,2);
//        A1(4*i+1,3*i)=WQ(1,0); A1(4*i+1,3*i+1)=WQ(1,1); A1(4*i+1,3*i+2) = WQ(1,2);
//        A1(4*i+2,3*i)=WQ(2,0); A1(4*i+2,3*i+1)=WQ(2,1); A1(4*i+2,3*i+2) = WQ(2,2);
//        A1(4*i+3,3*i)=WQ(3,0); A1(4*i+3,3*i+1)=WQ(3,1); A1(4*i+3,3*i+2) = WQ(3,2);
//    }
//    //std::cout<<A1<<std::endl;
//    
//    Eigen::MatrixXf A2(4*n, 2);
//    Eigen::MatrixXf t(4*n, 1);
//    
//    Eigen::MatrixXf B1(6,3*n);
//    for (int i = 0; i < n; i++) {
//        B1(0,3*i)=1; B1(0,3*i+1)=0; B1(0,3*i+2)=0;
//        B1(1,3*i)=0; B1(1,3*i+1)=1; B1(1,3*i+2)=0;
//        B1(2,3*i)=0; B1(2,3*i+1)=0; B1(2,3*i+2)=1;
//        B1(3,3*i)=0; B1(3,3*i+1)=-r(2,i); B1(3,3*i+2)=r(1,i);
//        B1(4,3*i)=r(2,i); B1(4,3*i+1)=0; B1(4,3*i+2)=-r(0,i);
//        B1(5,3*i)=-r(1,i); B1(5,3*i+1)=r(0,i); B1(5,3*i+2)=0;
//    }
//    //std::cout<<B1<<std::endl;
//    
//    Eigen::Matrix3f Tmg;
//    Tmg<< 0, -g(2), g(1),
//          g(2), 0, -g(0),
//          -g(1), g(0), 0;
//    Tmg *= M;
//    
//    Eigen::Matrix<float, 6, 2> B2;//Initialize with 0's???? use ::Zero
//    Eigen::Matrix<float, 3, 2> TmgP = -Tmg * P.transpose();
//    B2(3,0)=TmgP(0,0); B2(3,1)=TmgP(0,1);
//    B2(4,0)=TmgP(1,0); B2(4,1)=TmgP(1,1);
//    B2(5,0)=TmgP(2,0); B2(5,1)=TmgP(2,1);
//    std::cout<<B2<<std::endl;
//    
//    Eigen::Matrix<float, 6, 1> u;
//    u(0,0) = -M*g(0);
//    u(1,0) = -M*g(1);
//    u(2,0) = -M*g(2);
//    std::cout<<u<<std::endl;
//    
//    // get three vertices
//    /*iY = 0;
//    
//    Yin = [];
//    Youta = [];
//    Youtb = [];
//    
//    
//    %get three vertices
//    
//    A = [A1, A2];
//    B = [B1 B2];
//    f = zeros(3*n+2,1);
//    
//    a = [1;1];
//    a = a/norm(a);*/
//    
//    int iY = 0;
//    
//    Eigen::MatrixXf A(4*n,3*n+2);
//    A << A1, A2;
//    
//    Eigen::MatrixXf B(6,3*n+2);
//    B << B1, B2;
//    
//    //std::cout<<A<<std::endl;
//    //std::cout<<B<<std::endl;
//    
//    Eigen::MatrixXf f(3*n+2,1);
//    
//    Eigen::Vector2f a(1.0f,1.0f);
//    a.normalize();
//    
//    /*f(3*n+1:3*n+2,1) = a;
//    sol = linprog(-f,A,t,B,u);
//    
//    zopt = sol(3*n+1:3*n+2,1);
//    
//    iY = iY+1 ;
//    Yin(:,iY) = zopt;
//    Youta(:,iY) = a;
//    Youtb(iY) = transpose(a)*zopt;*/
//    
//    f(3*n,1) = a(0);
//    f(3*n+1,1) = a(1);
//    
//    Eigen::VectorXf sol(3*n+2);
//    //sol = linprog(-f,A,t,B,u);
//    
//    Eigen::Vector2f zopt(sol(3*n),sol(3*n+1));
//    
//    iY++;
//    std::vector<Eigen::Vector2f> YinTemp= std::vector<Eigen::Vector2f>();
//    YinTemp.push_back(zopt);
//    
//    std::vector<Eigen::Vector2f> YoutaTemp= std::vector<Eigen::Vector2f>();
//    YoutaTemp.push_back(a);
//    
//    std::vector<float> YoutbTemp= std::vector<float>();
//    YoutbTemp.push_back(a.transpose()*zopt);
//    
//    /*a = [-1;1];
//    a = a/norm(a);
//    
//    f(3*n+1:3*n+2,1) = a;
//    sol = linprog(-f,A,t,B,u);
//    
//    zopt = sol(3*n+1:3*n+2,1);
//    
//    iY = iY+1;
//    Yin(:,iY) = zopt;
//    Youta(:,iY) = a;
//    Youtb(iY) = transpose(a)*zopt;
//    
//    a = [0;-1];
//    a = a/norm(a);
//    
//    f(3*n+1:3*n+2,1) = a;
//    sol = linprog(-f,A,t,B,u);
//    
//    zopt = sol(3*n+1:3*n+2,1);
//    
//    iY = iY+1;
//    Yin(:,iY) = zopt;
//    Youta(:,iY) = a;
//    Youtb(iY) = transpose(a)*zopt;*/
//    
//    a = Eigen::Vector2f(-1.0f,1.0f);
//    a.normalize();
//    
//    f(3*n,1) = a(0);
//    f(3*n+1,1) = a(1);
//    //sol = linprog(-f,A,t,B,u);
//    
//    zopt(sol(3*n),sol(3*n+1));
//    
//    iY++;
//    YinTemp.push_back(zopt);
//    YoutaTemp.push_back(a);
//    YoutbTemp.push_back(a.transpose()*zopt);
//    
//    a = Eigen::Vector2f(0.0f,-1.0f);
//    a.normalize();
//    
//    f(3*n,1) = a(0);
//    f(3*n+1,1) = a(1);
//    //sol = linprog(-f,A,t,B,u);
//    
//    zopt(sol(3*n),sol(3*n+1));
//    
//    iY++;
//    YinTemp.push_back(zopt);
//    YoutaTemp.push_back(a);
//    YoutbTemp.push_back(a.transpose()*zopt);
//    
//    /*for i=1:1:iY
//        ang(1,i) = atan2(Yin(2,i),Yin(1,i));
//    end
//    [~,I] = sort(ang(1,:));
//    copyYin   = Yin;
//    copyYouta = Youta;
//    copyYoutb = Youtb;
//    for i=1:1:iY
//        Yin(:,i)   = copyYin(:,I(1,i));
//    Youta(:,i) = copyYouta(:,I(1,i));
//    Youtb(i)   = copyYoutb(I(1,i));
//    end*/
//    
//    Eigen::VectorXf ang(iY);
//    for (int i = 0; i < iY; i++) {
//       ang(i) = atan2f(YinTemp[1](i),YinTemp[0](i)); //ACCESS YinTemp differently
//    }
//    
//    //Sort by ang...
//    std::vector<int> I = std::vector<int>();
//    int rank = 0;
//    for (int i = 0; i < ang.count(); i++){
//        I.push_back(rank);
//        rank++;
//    }
//    bool switched = true;
//    while (switched) {
//        switched = false;
//        for (int i = 0; i < ang.count(); i++){
//            if (i < ang.count()-1) {
//                float curr = ang(i);
//                if (curr > ang(i+1)) {
//                    ang(i) = ang(i+1);
//                    ang(i+1) = curr;
//                    switched = true;
//                    int currIndex = I[i];
//                    I[i] = I[i+1];
//                    I[i+1] = currIndex;
//                }
//            }
//        }
//    }
//    std::vector<Eigen::Vector2f> copyYin = YinTemp;
//    std::vector<Eigen::Vector2f> copyYouta = YoutaTemp;
//    std::vector<float> copyYoutb = YoutbTemp;
//    for (int i=0; i < iY; i++) {
//        YinTemp[I[i]] = copyYin[i];
//        YoutaTemp[I[i]] = copyYouta[i];
//        YoutbTemp[I[i]] = copyYoutb[i];
//    }
//    
//    /*deltaA = 0;
//    
//    for i=1:1:(iY-1)
//        Yina(:,i) = [(Yin(2,i+1)-Yin(2,i)),-((Yin(1,i+1)-Yin(1,i)))];
//        Yina(:,i) = Yina(:,i)/norm(Yina(:,i));
//    
//        Yout(:,i) =  (1/( Youta(1,i)*Youta(2,i+1)-Youta(2,i)*Youta(1,i+1) ))*[Youta(2,i+1),-Youta(2,i);-Youta(1,i+1),Youta(1,i)]*[Youtb(i);Youtb(i+1)];
//        Aout(i) = polyarea([Yin(1,i:i+1),Yout(1,i)],[Yin(2,i:i+1),Yout(2,i)]);
//    end
//    Yina(:,iY) = [(Yin(2,1)-Yin(2,iY)),-((Yin(1,1)-Yin(1,iY)))];
//    Yina(:,iY) = Yina(:,iY)/norm(Yina(:,iY));
//    
//    Yout(:,iY) =  (1/( Youta(1,iY)*Youta(2,1)-Youta(2,iY)*Youta(1,1) ))*[Youta(2,1),-Youta(2,iY);-Youta(1,1),Youta(1,iY)]*[Youtb(iY);Youtb(1)];
//    Aout(iY) = polyarea([Yin(1,iY),Yin(1,1),Yout(1,iY)],[Yin(2,iY),Yin(2,1),Yout(2,iY)]);
//    
//    
//    deltaA = sum(Aout);*/
//    
//    float deltaA = 0.0f;
//    Eigen::Matrix2f Yina = Eigen::Matrix2f();
//    Eigen::Matrix2f Yout = Eigen::Matrix2f();
//    std::vector<float> Aout = std::vector<float>();
//    for (int i = 0; i < iY -1; i++) {
//        Eigen::Vector2f column = Eigen::Vector2f (Yin(1,i+1)-Yin(1,i),-(Yin(0,i+1)-Yin(0,i)));
//        column.normalize();
//        Yina.col(i) = column;
//        Eigen::Matrix2f temp;
//        temp << YoutaTemp[1](i+1), - YoutaTemp[1](i),
//                -YoutaTemp[0](i+1), YoutaTemp[0](i);
//        Eigen::Vector2f YoutCol =temp * Eigen::Vector2f(YoutbTemp[i],YoutbTemp[i+1]);
//        Yout.col(i) = (1/(YoutaTemp[0](i)*YoutaTemp[1](i+1)-YoutaTemp[1](i)*YoutaTemp[0](i+1)))* YoutCol;
//        //Aout.push_back(polyarea([Yin(1,i:i+1),Yout(1,i)],[Yin(2,i:i+1),Yout(2,i)]);));
//    }
//    Eigen::Vector2f column = Eigen::Vector2f (Yin(1,0)-Yin(1,iY),-(Yin(0,0)-Yin(0,iY)));
//    column.normalize();
//    Yina.col(iY) = column;
//    
//    Eigen::Matrix2f temp;
//    temp << YoutaTemp[1](0), - YoutaTemp[1](iY),
//    -YoutaTemp[0](0), YoutaTemp[0](iY);
//    Eigen::Vector2f YoutCol =temp * Eigen::Vector2f(YoutbTemp[iY],YoutbTemp[0]);
//    Yout.col(iY) = (1/(YoutaTemp[0](iY)*YoutaTemp[1](0)-YoutaTemp[1](iY)*YoutaTemp[0](0)))* YoutCol;
//    //Aout.push_back(polyarea([Yin(1,iY),Yin(1,1),Yout(1,iY)],[Yin(2,iY),Yin(2,1),Yout(2,iY)]));
//    for(int i = 0; i < Aout.size(); i++) {
//        deltaA += Aout[i];
//    }
//    
//    /*while deltaA>epsilon
//        
//        [~,i] = max(Aout);
//        a = Yina(:,i);
//        f(3*n+1:3*n+2,1) = a;
//        sol = linprog(-f,A,t,B,u);
//
//        zopt = sol(3*n+1:3*n+2,1);
//
//        if i<iY
//            Yin(:,i+2:iY+1) = Yin(:,i+1:iY);
//        Yina(:,i+2:iY+1) = Yina(:,i+1:iY);
//
//        Yout(:,i+2:iY+1) = Yout(:,i+1:iY);
//        Youta(:,i+2:iY+1) = Youta(:,i+1:iY);
//        Youtb(i+2:iY+1) = Youtb(i+1:iY);
//        end
//
//        Yin(:,i+1) = zopt;
//        Youta(:,i+1) = a;
//        Youtb(i+1) = transpose(a)*zopt;
//
//        iY = iY+1;
//
//
//        deltaA = 0;
//
//        Yina(:,i) = [(Yin(2,i+1)-Yin(2,i)),-((Yin(1,i+1)-Yin(1,i)))];
//        Yina(:,i) = Yina(:,i)/norm(Yina(:,i));
//
//        Yout(:,i) =  (1/( Youta(1,i)*Youta(2,i+1)-Youta(2,i)*Youta(1,i+1) ))*[Youta(2,i+1),-Youta(2,i);-Youta(1,i+1),Youta(1,i)]*[Youtb(i);Youtb(i+1)];
//
//        i = i+1;
//
//        if i+1>iY
//            Yina(:,iY) = [(Yin(2,1)-Yin(2,iY)),-((Yin(1,1)-Yin(1,iY)))];
//        Yina(:,iY) = Yina(:,iY)/norm(Yina(:,iY));
//
//        Yout(:,iY) =  (1/( Youta(1,iY)*Youta(2,1)-Youta(2,iY)*Youta(1,1) ))*[Youta(2,1),-Youta(2,iY);-Youta(1,1),Youta(1,iY)]*[Youtb(iY);Youtb(1)];
//        else
//            Yina(:,i) = [(Yin(2,i+1)-Yin(2,i)),-((Yin(1,i+1)-Yin(1,i)))];
//        Yina(:,i) = Yina(:,i)/norm(Yina(:,i));
//
//        Yout(:,i) =  (1/( Youta(1,i)*Youta(2,i+1)-Youta(2,i)*Youta(1,i+1) ))*[Youta(2,i+1),-Youta(2,i);-Youta(1,i+1),Youta(1,i)]*[Youtb(i);Youtb(i+1)];
//        end
//
//        for i=1:1:iY-1
//            Aout(i) = polyarea([Yin(1,i:i+1),Yout(1,i)],[Yin(2,i:i+1),Yout(2,i)]);
//        end
//        Aout(iY) = polyarea([Yin(1,iY),Yin(1,1),Yout(1,iY)],[Yin(2,iY),Yin(2,1),Yout(2,iY)]);
//
//        deltaA = sum(Aout);
//    
//    end*/
//    int i = 0;//??
//    //while (deltaA > epsilon) {
//        
//        
//   // }
//    
//    //eigen::
//
//    
//
//    
//    
//    
//    
//    
//    
//    
//    
//
//    
//    
//    
    
    return true;
}