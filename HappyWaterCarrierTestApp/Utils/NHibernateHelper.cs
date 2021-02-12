using HappyWaterCarrierTestApp.Model;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System;
using System.IO;
using System.Threading.Tasks;
using NHibernate.Context;
using System.Collections.Generic;

namespace HappyWaterCarrierTestApp
{
    class NHibernateHelper
    {
        private static volatile NHibernateHelper instance;
        private static readonly Object sync = new Object();

        private ISessionFactory sessionFactory;
        private ISession OpenSession()
        {
            if (CurrentSessionContext.HasBind(sessionFactory))
                return sessionFactory.GetCurrentSession();

            var session = sessionFactory.OpenSession();
            CurrentSessionContext.Bind(session);

            return sessionFactory.GetCurrentSession();
        }
        private NHibernateHelper() 
        {
            var configuration = new Configuration();
            var configurePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Model/NHibernate/NHibernate.cfg.xml");
            configuration = configuration.Configure(configurePath);
            configuration.AddAssembly(typeof(Item).Assembly);
            sessionFactory = configuration.BuildSessionFactory();
            new SchemaUpdate(configuration).Execute(true, true);
            
        }

        public static NHibernateHelper GetInstance()
        {
            if(instance == null)
            {
                lock (sync)
                {
                    if(instance == null)
                    {
                        instance = new NHibernateHelper();
                    }
                }
            }
            return instance;
        }

        public ISession GetSession()
        {
            ISession session = null;
            lock (sync)
            {
                session = OpenSession();
            }
            return session;
        }
        public void SaveAsync<T>(T o, Action<T> callback)
            where T : class
        {
            Task.Run(async () =>
            {
                var session = GetSession();
                using (var transaction = session.BeginTransaction())
                {
                    object id = await session.SaveAsync(o);
                    await transaction.CommitAsync();
                    callback(await session.GetAsync<T>(id));
                }
            });
        }
        public void DeleteAsync<T>(T o, Action<T> callback)
        {
            Task.Run(async () =>
            {
                var session = GetSession();
                using (var transaction = session.BeginTransaction())
                {
                    await session.DeleteAsync(o);
                    await transaction.CommitAsync();
                    callback(o);
                }
            });
        }

        public IList<T> GetItems<T>(int pageNumber, int pageSize) where T : class
        {
            var session = GetSession();
            {
                return session.QueryOver<T>()
                    .Take(pageSize)
                    .Skip(pageNumber*pageSize)
                    .List();
            }
            
        }
        public void GetItemsAsync<T>(int pageNumber, int pageSize, Action<IList<T>> callback, bool appendExtraItems = false) where T : class
        {
            Task.Run(async () =>
            {
                var session = GetSession();
                {
                    appendExtraItems = appendExtraItems && session.QueryOver<T>().RowCount() - (pageNumber + 1) * pageSize < pageSize;
                    if (appendExtraItems)
                    {
                        callback(await session.QueryOver<T>()
                            .Skip(pageNumber * pageSize)
                            .ListAsync());
                    }
                        
                    else
                    {
                        callback(await session.QueryOver<T>()
                            .Take(pageSize)
                            .Skip(pageNumber * pageSize)
                            .ListAsync());
                    }
                        
                }
            });
        }
        public int GetCount<T>() where T : class
        {
            return GetSession().QueryOver<T>().RowCount();
        }
        public int GetPagesCount<T>(int pageSize) where T : class
        {
            var session = GetSession();
            { 
                return (int)Math.Floor((double)session.QueryOver<T>().RowCount() / pageSize);
            }
        }
        public async void Update<T>(T obj)
        {
            await Task.Run(async () =>
            {
                var session = GetSession();
                using (var transaction = session.BeginTransaction())
                {
                    await session.UpdateAsync(obj);
                    await transaction.CommitAsync();
                }
            });
        }

        public IList<T> Get<T>(int pos, int length) where T : class
        {
            var session = GetSession();
            {
                return session.QueryOver<T>()
                    .Take(length)
                    .Skip(pos)
                    .List();
            }

        }
        public void GetAsync<T>(int pos, int length, Action<IList<T>> callback) where T : class
        {
            if (pos < 0)
            {
                length += pos;
                pos = 0;
            }
            if (length <= 0 || pos >= GetCount<T>())
                return;
            
            Task.Run(async () =>
            {
                
                var session = GetSession();
                {
                        callback(await session.QueryOver<T>()
                            .Take(length)
                            .Skip(pos)
                            .ListAsync());
                }
            });
        }
    }
}
