#include "..\PhysicsHandler.h"
#include "../../GameLoop/GameLoop.h"
#include "../../Objects/Ball.h"
#include "../CollisionDetector/CollisionDetector.h"

PhysicsHandler::PhysicsHandler() :
    m_collisionDetector(std::make_unique<CollisionDetector>())
{}

void PhysicsHandler::Visit(Ball& ball)
{
    float deltaTime = GameLoop::GetInstance().GetDeltaTime();
    sf::Vector2f ds = 
    {
        ball.GetDirection().x* ball.GetSpeed()* deltaTime,
        ball.GetDirection().y* ball.GetSpeed()* deltaTime
    };
    ball.Move(ds);
}

void PhysicsHandler::ResolveCollisions() const
{
    m_collisionDetector->ResolveCollisions();
}
