#include "..\GameLoop.h"

#include "../../MiddleAverageFilter.h"
#include "../../Objects/Ball.h"

GameLoop::GameLoop()
{
    m_renderer = std::make_unique<Renderer>(Renderer::WindowSettings({ WINDOW_W, WINDOW_H, "ball collision demo" }));
    m_physicsHandler = std::make_unique<PhysicsHandler>();
    m_lastTime = m_clock.restart().asSeconds();
}

void GameLoop::SetRunSettings(int maxBalls, int minBalls, bool enableFpsCounter)
{
    m_maxBalls = maxBalls;
    m_minBalls = minBalls;
    m_enableFpsCounter = enableFpsCounter;
}

void GameLoop::SetRadiusRule(std::function<float()> radiusRule)
{
    m_radiusRule = radiusRule;
}

void GameLoop::SetSpeedRule(std::function<float()> speedRule)
{
    m_speedRule = speedRule;
}

void GameLoop::SetDirectionRule(std::function<sf::Vector2f()> directionRule)
{
    m_directionRule = directionRule;
}

void GameLoop::SetPositionRule(std::function<sf::Vector2f()> positionRule)
{
    m_positionRule = positionRule;
}

void GameLoop::Run()
{
    srand(time(NULL));
    int minMaxDiff = m_maxBalls - m_minBalls;
    for (int i = 0; i < ((minMaxDiff > 0 ? rand() % (minMaxDiff) : 0) + m_minBalls); ++i)
    {
        m_objects.push_back(CreateRandomBall());
    }

    for (auto object : m_objects)
    {
        if (auto pBall = static_cast<Ball*>(object.get()))
        {
            float radius = pBall->GetRadius();
            if (radius > m_maxRadius)
                m_maxRadius = radius;
        }
    }

    Math::MiddleAverageFilter<float, 100> fpscounter;

    while (m_renderer->IsWindowOpen())
    {
        sf::Event event;
        while (m_renderer->PollEvent(event))
        {
            if (event.type == sf::Event::Closed)
            {
                m_renderer->CloseWindow();
            }
        }

        UpdateTime();
        if(m_enableFpsCounter)
            fpscounter.push(1.0f / GetDeltaTime());

        SortObjectsByX();
        for (const auto object : m_objects)
        {
            object->accept(*m_physicsHandler);
        }
        m_physicsHandler->ResolveCollisions();

        m_renderer->Clear();
        for (const auto object : m_objects)
        {
            object->accept(*m_renderer);
        }

        if(m_enableFpsCounter)
            m_renderer->SetTitle(std::string("FPS: ") + std::to_string(fpscounter.getAverage()));
        m_renderer->Display();
    }
}

void GameLoop::UpdateTime()
{
    float current_time = m_clock.getElapsedTime().asSeconds();
    m_deltaTime = current_time - m_lastTime;
    m_lastTime = current_time;
}

float GameLoop::GetDeltaTime()
{
    return m_deltaTime;
}

const Renderer::WindowSettings& GameLoop::GetWindowSettings() const
{
    return m_renderer->GetWindowSettings();
}

std::list<std::shared_ptr<Object>> GameLoop::GetObjects()
{
    return m_objects;
}

inline std::shared_ptr<Ball> GameLoop::CreateRandomBall() const
{
    return std::make_shared<Ball>(m_positionRule(), m_radiusRule(), m_directionRule(), m_speedRule());
}

void GameLoop::SortObjectsByX()
{
    // Rework sorting if add new object type
    m_objects.sort([](std::shared_ptr<Object> a, std::shared_ptr<Object> b)
        {
            if (auto pBall = static_cast<Ball*>(a.get()))
            {
                if (auto pOtherBall = static_cast<Ball*>(b.get()))
                {
                    return pBall->GetPosition().x > pOtherBall->GetPosition().x;
                }
            }
            return a.get() > b.get();
        });
}

float GameLoop::GetMaxRadius() const
{
    return m_maxRadius;
}

GameLoop::~GameLoop()
{
}
