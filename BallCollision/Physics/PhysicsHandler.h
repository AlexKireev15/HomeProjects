#pragma once
#include <memory>

#include "../Objects/ObjectVisitor.h"
#include "CollisionDetector/CollisionDetector.h"
class PhysicsHandler : public ObjectVisitor
{
public:
    PhysicsHandler();
    void Visit(Ball& b);
    void ResolveCollisions() const;
private:
    std::unique_ptr<CollisionDetector> m_collisionDetector;
};