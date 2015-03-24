//
//  mpFABRIK.cpp
//  Motion Path Planner
//
//  Created by Max Gilbert on 3/2/15.
//  Copyright (c) 2015 UPenn. All rights reserved.
//
#include "mpFABRIK.h"
std::vector<Eigen::Vector3f> mpFABRIK::solveIK(std::vector<Eigen::Vector3f> P, Eigen::Vector3f T, std::vector<float> D){
    float dist = (P[0]-T).norm();
    int numJoints = P.size();
    float totalD = 0.0f;
    for(int i = 0; i < D.size(); i++){
        totalD+=D[i];
    }
    if(dist > totalD){
        for(int i = 0; i<numJoints-1;i++){
            float ri = (T-P[i]).norm();
            float ki = D[i]/ri;
            P[i+1]=(1-ki)*P[i]+ki*T;
        }
    } else {
        Eigen::Vector3f b = P[0];
        float diffA = (P[numJoints-1]-T).norm();
        int iterations = 0;
        while(diffA > tol && iterations <= maxIterations){
            P[numJoints-1] = T;
            for (int i = numJoints-2; i>=0; i--) {
                float ri = (P[i+1]-P[i]).norm();
                float ki = D[i]/ri;
                P[i]=(1-ki)*P[i+1]+ki*P[i];
            }
            P[0] = b;
            for (int i = 0; i < numJoints-1; i++) {
                float ri = (P[i+1]-P[i]).norm();
                float ki = D[i]/ri;
                P[i+1]=(1-ki)*P[i]+ki*P[i+1];
            }
            diffA = (P[numJoints-1]-T).norm();
            iterations++;
        }
    }
    return P;
}
