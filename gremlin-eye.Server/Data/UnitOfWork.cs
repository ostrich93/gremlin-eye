﻿using gremlin_eye.Server.Repositories;

namespace gremlin_eye.Server.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;

        private IUserRepository? _userRepository;
        private IGameLogRepository? _logRepository;
        private IGameRepository? _gameRepository;
        private IPlaythroughRepository? _playthroughRepository;
        private IReviewRepository? _reviewRepository;
        private IListingRepository? _listingRepository;
        private ILikeRepository? _likeRepository;
        public DataContext Context { get { return _context; } }

        public UnitOfWork(DataContext context)
        {
            _context = context;
        }

        public IUserRepository Users
        {
            get 
            {
                if (_userRepository == null)
                    _userRepository = new UserRepository(_context);
                return _userRepository;
            }
        }

        public IGameLogRepository GameLogs
        {
            get
            {
                if (_logRepository == null)
                {
                    _logRepository = new GameLogRepository(_context);
                }
                return _logRepository;
            }
        }

        public IGameRepository Games
        {
            get
            {
                if (_gameRepository == null)
                {
                    _gameRepository = new GameRepository(_context);
                }
                return _gameRepository;
            }
        }

        public IPlaythroughRepository Playthroughs
        {
            get
            {
                if (_playthroughRepository == null)
                {
                    _playthroughRepository = new PlaythroughRepository(_context);
                }
                return _playthroughRepository;
            }
        }

        public IListingRepository Lists
        {
            get
            {
                if (_listingRepository == null)
                {
                    _listingRepository = new ListingRepository(_context);
                }
                return _listingRepository;
            }
        }

        public IReviewRepository Reviews
        {
            get
            {
                if (_reviewRepository == null)
                {
                    _reviewRepository = new ReviewRepository(_context);
                }
                return _reviewRepository;
            }
        }

        public ILikeRepository Likes
        {
            get
            {
                if (_likeRepository == null)
                {
                    _likeRepository = new LikeRepository(_context);
                }
                return _likeRepository;
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual async ValueTask DisposeAsync(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    await _context.DisposeAsync();
                }
            }
            disposed = true;
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }
    }
}
