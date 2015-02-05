//
//  mpHeap.h
//  Motion Path Planner
//
//  Created by Max Gilbert on 1/30/15.
//  Copyright (c) 2015 UPenn. All rights reserved.
//

#ifndef __Motion_Path_Planner__mpHeap__
#define __Motion_Path_Planner__mpHeap__

#include <stdio.h>
#include <vector>
template <class T> class mpHeap{
    
private:
    std::vector<T> _array;
    void Heapify(int index){
        T minChild = _array[2*index + 1];
        if (_array[2*index + 2] < minChild) minChild = _array[2*index+2];
        
    }
    int size = 0;
public:
    mpHeap (){
        _array = std::vector<T>();
    }
    mpHeap (const mpHeap& other);
    mpHeap& operator=( const mpHeap& rhs );
    ~mpHeap(){
        
    }
    void Add (T element){
        _array.push_back(element);
        int index = _array.size()-1;
        T parent = _array[(index-1)/2];
        while (element < parent) {
            _array[index] = parent;
            index = (index-1)/2;
            _array[index] = element;
            parent =_array[(index-1)/2];
        }
    }
    void RemoveAt (int index){
        
    }
    void RemoveElement (T element);
    T LookAt (){
        if (_array.size() > 0){
            return _array[0];
        } else {
            //TODO throw error
        }
        return NULL;
    }
    T Extract (){
        T top = LookAt();
        RemoveAt(0);
        return top;
    }
    bool maxHeap = false;
};

#endif /* defined(__Motion_Path_Planner__mpHeap__) */
