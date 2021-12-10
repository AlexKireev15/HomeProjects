#pragma once
class Ball;
class ObjectVisitor
{
public:
    virtual void Visit(Ball& b) = 0;
};