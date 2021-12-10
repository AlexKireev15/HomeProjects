#include "../Renderer.h"
#include "../../Objects/Ball.h"

void Renderer::Visit(Ball& ball)
{
    m_window.draw(ball.GetGraphics());
}

void Renderer::Clear()
{
    m_window.clear();
}

void Renderer::Display()
{
    m_window.display();
}

bool Renderer::PollEvent(sf::Event& event)
{
    return m_window.pollEvent(event);
}

void Renderer::CloseWindow()
{
    m_window.close();
}

void Renderer::SetTitle(const std::string& title)
{
    m_window.setTitle(title);
}

bool Renderer::IsWindowOpen() const
{
    return m_window.isOpen();
}

const Renderer::WindowSettings& Renderer::GetWindowSettings() const
{
    return m_settings;
}
