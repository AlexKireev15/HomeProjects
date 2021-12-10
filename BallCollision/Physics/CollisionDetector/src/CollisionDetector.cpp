#include "..\CollisionDetector.h"
#include "../../../GameLoop/GameLoop.h"
#include "../../../Objects/Ball.h"

sf::Vector2f NegateDirectionX(const sf::Vector2f& direction)
{
    if (direction.x > 0)
    {
        auto result = direction;
        result.x = -result.x;
        return result;
    }
    return direction;
}

sf::Vector2f NegateDirectionY(const sf::Vector2f& direction)
{
    if (direction.y > 0)
    {
        auto result = direction;
        result.y = -result.y;
        return result;
    }
    return direction;
}

sf::Vector2f UnnegateDirectionX(const sf::Vector2f& direction)
{
    if (direction.x <= 0)
    {
        auto result = direction;
        result.x = -result.x;
        return result;
    }
    return direction;
}

sf::Vector2f UnnegateDirectionY(const sf::Vector2f& direction)
{
    if (direction.y <= 0)
    {
        auto result = direction;
        result.y = -result.y;
        return result;
    }
    return direction;
}

float GetLength(const sf::Vector2f& vector)
{
    return sqrt(vector.x * vector.x + vector.y * vector.y);
}

sf::Vector2f Unify(const sf::Vector2f& v)
{
    return v / GetLength(v);
}

float DotProduct(const sf::Vector2f& v1, const sf::Vector2f& v2)
{
    return v1.x * v2.x + v1.y * v2.y;
}

void HandleCollision(Ball* pBall, Ball* pOtherBall)
{
    auto errorVector = pBall->GetCenter() - pOtherBall->GetCenter();
    errorVector = Unify(errorVector) * ((pBall->GetRadius() + pOtherBall->GetRadius()) - GetLength(errorVector));

    pBall->Move(errorVector/2.f);
    pOtherBall->Move(-errorVector/2.f);

    const sf::Vector2f axisX = Unify(pOtherBall->GetCenter() - pBall->GetCenter());
    const sf::Vector2f u1x = axisX * DotProduct(axisX, pBall->GetDirection() * pBall->GetSpeed());
    const sf::Vector2f u1y = pBall->GetDirection() * pBall->GetSpeed() - u1x;
    const sf::Vector2f u2x = -axisX * DotProduct(-axisX, pOtherBall->GetDirection() * pOtherBall->GetSpeed());
    const sf::Vector2f u2y = pOtherBall->GetDirection() * pOtherBall->GetSpeed() - u2x;
    const float& m1 = pBall->GetMass();
    const float& m2 = pOtherBall->GetMass();
    
    const sf::Vector2f v1x = ((u1x * m1) + (u2x * m2) - (u1x - u2x) * m2) / (m1 + m2);
    const sf::Vector2f v2x = ((u1x * m1) + (u2x * m2) - (u2x - u1x) * m1) / (m1 + m2);
    const sf::Vector2f v1y = u1y;
    const sf::Vector2f v2y = u2y;

    pBall->SetDirection(Unify(v1x + v1y));
    pBall->SetSpeed(GetLength(v1x + v1y));
    pOtherBall->SetDirection(Unify(v2x + v2y));
    pOtherBall->SetSpeed(GetLength(v2x + v2y));
}

void CollisionDetector::ResolveCollisions() const
{
    auto objects = GameLoop::GetInstance().GetObjects();
    float maxRadius = GameLoop::GetInstance().GetMaxRadius();

    for (auto it = objects.begin(); it != objects.end(); it = objects.erase(it))
    {
        auto object = *it;
        auto pBall = static_cast<Ball*>(object.get());
        if (!pBall)
            continue;
        ResolveScreenCollisions(pBall);

        auto TryHandleCollision = [&pBall, &object, &maxRadius, this](auto it)
        {
            auto other = *it;
            if (object == other)
                return false;
            auto pOtherBall = static_cast<Ball*>(other.get());
            if (!pOtherBall)
                return false;

            float xDistance = abs(pBall->GetCenter().x - pOtherBall->GetCenter().x);
            if (xDistance > 3.f * maxRadius)
                return true;

            float distance = GetLength(pBall->GetCenter() - pOtherBall->GetCenter());
            if (distance <= pBall->GetRadius() + pOtherBall->GetRadius())
                HandleCollision(pBall, pOtherBall);
            return false;
        };

        for (auto forwardIt = it; forwardIt != objects.end(); ++forwardIt)
            if (TryHandleCollision(forwardIt))
                break;

        for (auto backIt = it; backIt != objects.begin(); --backIt)
            if (TryHandleCollision(backIt))
                break;
    }
}

void CollisionDetector::ResolveScreenCollisions(Ball* pBall) const
{
    auto& windowSettings = GameLoop::GetInstance().GetWindowSettings();
    if (pBall->GetPosition().x <= 0.0)
        pBall->SetDirection(UnnegateDirectionX(pBall->GetDirection()));
    if (pBall->GetPosition().x + 2 * pBall->GetRadius() >= windowSettings.m_width)
        pBall->SetDirection(NegateDirectionX(pBall->GetDirection()));

    if (pBall->GetPosition().y <= 0.0)
        pBall->SetDirection(UnnegateDirectionY(pBall->GetDirection()));
    if (pBall->GetPosition().y + 2 * pBall->GetRadius() >= windowSettings.m_height)
        pBall->SetDirection(NegateDirectionY(pBall->GetDirection()));
}
