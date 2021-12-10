#pragma once
#include "Lazy.h"

template <typename T>
class Singleton
{
    friend struct std::default_delete<Singleton<T>>;
    friend T* Init();
public:
    static T& GetInstance()
    {
        return *static_cast<T*>(m_instance.Get().get());;
    }
protected:
    Singleton()
    {
    }
    Singleton(const Singleton<T>&) = delete;
    void operator=(const Singleton<T>&) = delete;
    Singleton(Singleton<T>&&) = delete;
    void operator=(Singleton<T>&&) = delete;
    virtual ~Singleton() { }
    template<typename T>
    static T* Init()
    {
        return new T;
    }

    static Lazy<Singleton<T>> m_instance;
};

template <typename T>
Lazy<Singleton<T>> Singleton<T>::m_instance(Singleton<T>::Init<T>);