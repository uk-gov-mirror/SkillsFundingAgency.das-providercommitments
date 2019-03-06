﻿namespace SFA.DAS.ProviderCommitments.ModelBinding
{
    public interface IAuthorizationContext
    {
        T Get<T>(string key);

        void Set<T>(string key, T value);

        bool TryGet<T>(string key, out T value);
    }
}