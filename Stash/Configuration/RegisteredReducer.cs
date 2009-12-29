namespace Stash.Configuration
{
    using System;

    public class RegisteredReducer
    {
        public RegisteredReducer(Reducer reducer)
        {
            Reducer = reducer;
        }

        public Reducer Reducer { get; private set; }
    }
}