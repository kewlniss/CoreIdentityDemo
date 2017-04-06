using CoreIdentityDemo.Data.DapperSql.Repositories;
using CoreIdentityDemo.Domain.Repositories;
using CoreIdentityDemo.Domain.UnitOfWork;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace CoreIdentityDemo.Data.DapperSql.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;

        private IRoleClaimRepository _roleClaimRepository;
        private IRoleRepository _roleRepository;
        private IUserLoginRepository _userLoginRepository;
        private IUserRepository _userRepository;
        private IUserTokenRepository _userTokenRepository;
        private IUserClaimRepository _userClaimRepository;

        private bool _disposed;

        public UnitOfWork(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public IRoleClaimRepository RoleClaimRepository
        {
            get { return _roleClaimRepository ?? (_roleClaimRepository = new RoleClaimRepository(_transaction)); }
        }

        public IRoleRepository RoleRepository
        {
            get { return _roleRepository ?? (_roleRepository = new RoleRepository(_transaction)); }
        }

        public IUserClaimRepository UserClaimRepository
        {
            get { return _userClaimRepository ?? (_userClaimRepository = new UserClaimRepository(_transaction)); }
        }

        public IUserLoginRepository UserLoginRepository
        {
            get { return _userLoginRepository ?? (_userLoginRepository = new UserLoginRepository(_transaction)); }
        }

        public IUserRepository UserRepository
        {
            get { return _userRepository ?? (_userRepository = new UserRepository(_transaction)); }
        }

        public IUserTokenRepository UserTokenRepository
        {
            get { return _userTokenRepository ?? (_userTokenRepository = new UserTokenRepository(_transaction)); }
        }

        public void Commit()
        {
            CommitAsync().Wait();
        }

        public Task CommitAsync()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
                _transaction.Dispose();
                _transaction = _connection.BeginTransaction();
                resetRepositories();
            }
            return Task.CompletedTask;
        }

        private void resetRepositories()
        {
            _roleClaimRepository = null;
            _roleRepository = null;
            _userClaimRepository = null;
            _userLoginRepository = null;
            _userRepository = null;
            _userTokenRepository = null;
        }

        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }

        private void dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }
                    if (_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }
                }
                _disposed = true;
            }
        }

        ~UnitOfWork()
        {
            dispose(false);
        }
    }
}
