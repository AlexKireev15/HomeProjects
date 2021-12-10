#pragma once
#include <list>
#include <functional>

#include "../Utils/Singleton.h"

#include "../Renderer/Renderer.h"
#include "../Physics/PhysicsHandler.h"
#include "../Objects/Object.h"

typedef Renderer::WindowSettings WindowSettings;

constexpr int WINDOW_W = 800;
constexpr int WINDOW_H = 600;

class GameLoop : public Singleton<GameLoop>
{
    friend class Singleton<GameLoop>;
public:
    void Run();
    float GetDeltaTime();
    const WindowSettings& GetWindowSettings() const;
    float GetMaxRadius() const;

    std::list<std::shared_ptr<Object>> GetObjects();

    void SetRunSettings(int maxBalls = 300, int minBalls = 100, bool enableFpsCounter = false);
    void SetRadiusRule(std::function<float()> radiusRule);
    void SetSpeedRule(std::function<float()> speedRule);
    void SetDirectionRule(std::function<sf::Vector2f()> directionRule);
    void SetPositionRule(std::function<sf::Vector2f()> positionRule);

protected:
    GameLoop();
    void UpdateTime();
    virtual ~GameLoop();

private:
    inline std::shared_ptr<Ball> CreateRandomBall() const;

    void SortObjectsByX();

    int m_maxBalls = 300;
    int m_minBalls = 100;
    bool m_enableFpsCounter = false;
    std::function<float()> m_radiusRule = []()
    {
        return 5 + rand() % 5;
    };
    std::function<float ()> m_speedRule = []()
    {
        return 30 + rand() % 30;
    };
    std::function<sf::Vector2f ()> m_directionRule = []()
    {
        return sf::Vector2f(
            (float)((-5 + (rand() % 10)) / 3.),
            (float)((-5 + (rand() % 10)) / 3.)
        );
    };
    std::function<sf::Vector2f()> m_positionRule = []()
    {
        return sf::Vector2f(
            (float)(rand() % WINDOW_W),
            (float)(rand() % WINDOW_H)
        );
    };

    std::unique_ptr<Renderer> m_renderer;
    std::unique_ptr<PhysicsHandler> m_physicsHandler;
    std::list<std::shared_ptr<Object>> m_objects;
    sf::Clock m_clock;

    float m_lastTime;
    float m_deltaTime;
    float m_maxRadius = 0.f;
};