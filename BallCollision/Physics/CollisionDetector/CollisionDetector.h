#pragma once
#include <map>
#include <vector>
#include <memory>

#include "../../Objects/ObjectVisitor.h"
#include "../../Objects/Object.h"
class CollisionDetector
{
public:
    void ResolveCollisions() const;
private:
    void ResolveScreenCollisions(Ball* pBall) const;
};