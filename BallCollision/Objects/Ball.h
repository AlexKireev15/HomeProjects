#pragma once
#define _USE_MATH_DEFINES
#include <math.h>

#include "SFML/Graphics.hpp"

#include "Object.h"
#include "ObjectVisitor.h"
class Ball : public Object
{
public:
    Ball(const sf::Vector2f& position, const float& radius,
        const sf::Vector2f& direction, const float& speed) :
        m_direction(direction), m_speed(speed)
    {
        m_graphics.setPosition(position);
        m_graphics.setRadius(radius);
    }
    virtual void accept(ObjectVisitor& visitor)
    {
        visitor.Visit(*this);
    }
    const sf::CircleShape& GetGraphics() const
    {
        return m_graphics;
    }
    const sf::Vector2f& GetPosition() const
    {
        return m_graphics.getPosition();
    }
    sf::Vector2f GetCenter() const
    {
        auto center = m_graphics.getPosition();
        center.x += m_graphics.getRadius();
        center.y += m_graphics.getRadius();
        return center;
    }
    float GetMass() const
    {
        return m_graphics.getRadius() * m_graphics.getRadius() * M_PI;
    }
    const sf::Vector2f& GetDirection() const
    {
        return m_direction;
    }
    float GetSpeed() const
    {
        return m_speed;
    }
    float GetRadius() const
    {
        return m_graphics.getRadius();
    }
    void SetRadius(const float& radius)
    {
        m_graphics.setRadius(radius);
    }
    void SetPosition(const sf::Vector2f& position)
    {
        m_graphics.setPosition(position);
    }
    void Move(const sf::Vector2f& offset)
    {
        m_graphics.move(offset);
    }
    void SetDirection(const sf::Vector2f& direction)
    {
        m_direction = direction;
    }
    void SetSpeed(const float& speed)
    {
        m_speed = speed;
    }
private:
    sf::CircleShape m_graphics;
    sf::Vector2f m_direction;
    float m_speed = 0;
};