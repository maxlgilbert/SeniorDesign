//
//  supreg.h
//  AtlasResearchC++
//
//  Created by Max Gilbert on 9/19/14.
//  Copyright (c) 2014 UPenn. All rights reserved.
//

#ifndef __AtlasResearchC____supreg__
#define __AtlasResearchC____supreg__

#include <iostream>
#include "Eigen/Dense"

class Supreg {
public:
    bool SupportRegion (int n, const Eigen::MatrixXf &r, const Eigen::MatrixXf &nu,
                        float mus, float M, float epsilon, float &nvert, Eigen::MatrixXf &Yin,
                        int &nlines, Eigen::MatrixXf &lines);
    
};

#endif /* defined(__AtlasResearchC____supreg__) */
