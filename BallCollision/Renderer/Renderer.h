#pragma once
#include <string>

#include "SFML/Graphics.hpp"

#include "../Objects/ObjectVisitor.h"

class Renderer : public ObjectVisitor
{
public:
    struct WindowSettings
    {
        float m_width;
        float m_height;
        std::string m_title;
    };

    Renderer(const WindowSettings& settings) :
        m_window(sf::VideoMode(settings.m_width, settings.m_height), settings.m_title),
        m_settings(settings)
    { }
    virtual void Visit(Ball& b);

    void Clear();
    void Display();
    void CloseWindow();
    void SetTitle(const std::string& title);
    bool PollEvent(sf::Event& event);
    bool IsWindowOpen() const;
    const WindowSettings& GetWindowSettings() const;

    virtual ~Renderer() { }
    
private:
    sf::RenderWindow m_window;
    WindowSettings m_settings;
};