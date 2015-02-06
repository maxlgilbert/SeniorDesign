//
//  main.cpp
//  Motion Path Planner
//
//  Created by Max Gilbert on 1/30/15.
//  Copyright (c) 2015 UPenn. All rights reserved.
//

#include <iostream>
#include "mpHeap.h"
int main(int argc, const char * argv[]) {
    // insert code here...
    mpHeap<int> minHeap = mpHeap<int>();
    minHeap.Add(2);
    minHeap.Add(6);
    minHeap.Add(1);
    minHeap.Add(3);
    minHeap.Add(5);
    minHeap.Add(5);
    std::cout << minHeap.LookAt()<<std::endl;
    minHeap.RemoveAt(0);
    std::cout << minHeap.LookAt()<<std::endl;
    minHeap.RemoveElement(5);
    std::cout << minHeap.Extract()<<std::endl;
    std::cout << minHeap.Extract()<<std::endl;
    std::cout << minHeap.Extract()<<std::endl;
    std::cout << minHeap.LookAt()<<std::endl;
    return 0;
}
