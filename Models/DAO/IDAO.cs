using System;
using System.Collections.Generic;
using System.Text;
using UaiFuel.Models.Domain;

namespace UaiFuel.Models.DAO
{
    public interface IDAO<T> where T : IDomainObject, new()
    {
        public T Create(T domainObject);

        public void Update(T domainObject);

        public void Delete(T domainObject);

        public IList<T> Read();

        public T Read(object pk);

    }
}
