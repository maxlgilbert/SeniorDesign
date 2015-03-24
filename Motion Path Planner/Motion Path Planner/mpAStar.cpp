//
//  mpAStar.cpp
//  Motion Path Planner
//
//  Created by Max Gilbert on 2/5/15.
//  Copyright (c) 2015 UPenn. All rights reserved.
//

#include "mpAStar.h"

/*mpAStar::mpAStar(){
    
}

void mpAStar::findPath(mpAStarNode start, mpAStarNode goal, int maxDepth){
    mpHeap<mpAStarNode> potentialNodes = mpHeap<mpAStarNode>();
    potentialNodes.Add(start);
    int depth = 0;
    while (depth < maxDepth) {
        mpAStarNode curr = potentialNodes.Extract();
        if(curr == goal) {
            return;
        } else {
            std::vector<mpAStarNode> neighbors = curr.GetNeighbors();
            for (int i =0; i < neighbors.size(); i++) {
                //Check visited...
                neighbors[i].parentNode = &curr;
                potentialNodes.Add(neighbors[i]);
            }
        }
        depth++;
    }
}*/