using System.Collections.Generic;

namespace Mentoring.Core
{
    public class PagedResult<TEntity>
    {
        private readonly IEnumerable<TEntity> _items;
        private readonly int _totalCount;

        public PagedResult(IEnumerable<TEntity> items, int totalCount)
        {
            _items = items;
            _totalCount = totalCount;
        }

        public IEnumerable<TEntity> Items
        {
            get
            {
                return _items;
            }
        }

        public int TotalCount
        {
            get
            {
                return _totalCount;
            }
        }
    }
}