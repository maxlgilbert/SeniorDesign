//
//  mpFABRIK.h
//  Motion Path Planner
//
//  Created by Max Gilbert on 3/2/15.
//  Copyright (c) 2015 UPenn. All rights reserved.
//

#ifndef __Motion_Path_Planner__mpFABRIK__
#define __Motion_Path_Planner__mpFABRIK__

#include <vector>
#include <stdio.h>
#include "Eigen/Dense"
class mpFABRIK{
public:
    std::vector<Eigen::Vector3f> solveIK(std::vector<Eigen::Vector3f> P, Eigen::Vector3f T, std::vector<float> D);
private:
    float tol = .0001f;
    int maxIterations = 100;
    
};

#endif /* defined(__Motion_Path_Planner__mpFABRIK__) */
