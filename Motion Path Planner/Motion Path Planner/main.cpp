//
//  main.cpp
//  Motion Path Planner
//
//  Created by Max Gilbert on 1/30/15.
//  Copyright (c) 2015 UPenn. All rights reserved.
//

#include <iostream>
//#include "mpHeap.h"
#include "mpFABRIK.h"
int main(int argc, const char * argv[]) {
    // insert code here...
//    mpHeap<int> minHeap = mpHeap<int>();
//    minHeap.Add(2);
//    minHeap.Add(6);
//    minHeap.Add(1);
//    minHeap.Add(3);
//    minHeap.Add(5);
//    minHeap.Add(5);
//    std::cout << minHeap.LookAt()<<std::endl;
//    minHeap.RemoveAt(0);
//    std::cout << minHeap.LookAt()<<std::endl;
//    minHeap.RemoveElement(5);
//    std::cout << minHeap.Extract()<<std::endl;
//    std::cout << minHeap.Extract()<<std::endl;
//    std::cout << minHeap.Extract()<<std::endl;
//    std::cout << minHeap.LookAt()<<std::endl;
    std::vector<Eigen::Vector3f> P = std::vector<Eigen::Vector3f>();
    P.push_back(Eigen::Vector3f(0.0f,0.0f,0.0f));
    P.push_back(Eigen::Vector3f(0.0f,1.0f,0.0f));
    P.push_back(Eigen::Vector3f(0.0f,2.0f,0.0f));
    P.push_back(Eigen::Vector3f(0.0f,3.0f,0.0f));
    std::cout <<"Input"<< std::endl;
    for(int i =0; i < P.size(); i++){
        std::cout << P[i].x() << ", "<<P[i].y() << ", "<<P[i].z() << std::endl;
    }
    Eigen::Vector3f T = Eigen::Vector3f(2.0f,0.0f,0.0f);
    std::vector<float> D = std::vector<float>();
    for(int i =0; i < P.size()-1; i++){
        D.push_back((P[i+1]-P[i]).norm());
    }
    mpFABRIK ikChain = mpFABRIK();
    P = ikChain.solveIK(P, T, D);
    std::cout <<"Output"<< std::endl;
    for(int i =0; i < P.size(); i++){
        std::cout << P[i].x() << ", "<<P[i].y() << ", "<<P[i].z() << std::endl;
    }
    return 0;
}
