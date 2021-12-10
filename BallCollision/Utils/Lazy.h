#pragma once
#include <memory>
#include <mutex>
#include <functional>
template <typename T>
class Lazy
{
public:
    template <typename F>
    explicit Lazy(F&& initFunction) : m_initFunction(std::forward<F>(initFunction)) {}

    const std::unique_ptr<T>& Get()
    {
        std::call_once(m_onceFlag, [this]
        {
            m_value.reset(m_initFunction());
        });
        return m_value;
    }
private:
    std::once_flag m_onceFlag;
    std::unique_ptr<T> m_value;
    std::function<T* ()> m_initFunction;
};