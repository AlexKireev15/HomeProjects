#include "GameLoop/GameLoop.h"

int main(int argc, char** argv)
{
    GameLoop::GetInstance().SetRunSettings(500, 500, true);
    GameLoop::GetInstance().SetDirectionRule([]()
        {
            return sf::Vector2f(
                (float)((-5 + (rand() % 10)) / 3.),
                (float)((-5 + (rand() % 10)) / 3.)
            );
        });
    GameLoop::GetInstance().SetPositionRule([]()
        {
            return sf::Vector2f(
                (float)(rand() % WINDOW_W),
                (float)(rand() % WINDOW_H)
            );
        });
    GameLoop::GetInstance().SetSpeedRule([]()
        {
            return 30 + rand() % 30;
        });
    GameLoop::GetInstance().SetRadiusRule([]()
        {
            return 2 + rand() % 5;
        });
    GameLoop::GetInstance().Run();
    return 0;
}