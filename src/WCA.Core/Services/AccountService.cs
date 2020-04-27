using System;
using System.Collections.Generic;
using System.Text;
using WCA.Data;

namespace WCA.Core.Services
{
    public class AccountService
    {
        private readonly WCADbContext _wCADbContext;

        public AccountService(WCADbContext wCADbContext)
        {
            _wCADbContext = wCADbContext;
        }

    }
}
