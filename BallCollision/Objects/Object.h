#pragma once
#include "ObjectVisitor.h"
class Object
{
public:
   virtual void accept(ObjectVisitor& visitor) = 0;
};