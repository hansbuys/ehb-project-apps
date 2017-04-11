using System;
using System.Collections.Concurrent;
using System.Runtime.Remoting.Messaging;
using FluentAssertions;
using FluentAssertions.Primitives;

namespace Ehb.Dijlezonen.Kassa.Infrastructure.Testing
{
    public static class Assertions
    {
        private const string ContextKey = "assertions-testoutput-delegate";

        private static readonly ConcurrentDictionary<Guid, Action<string>> writeOutputDelegates =
            new ConcurrentDictionary<Guid, Action<string>>();

        public static Action<string> WriteTestOutput
        {
            set
            {
                var id = Guid.NewGuid();

                writeOutputDelegates.AddOrUpdate(id, value,
                    (guid, action) => { throw new Exception("Key already in use."); });

                CallContext.LogicalSetData(ContextKey, id);
            }
            internal get
            {
                var id = (Guid)CallContext.LogicalGetData(ContextKey);

                Action<string> value;
                if (!writeOutputDelegates.TryGetValue(id, out value))
                    throw new Exception("Unabled to find write-testoutput delegate for testing.");

                return value;
            }
        }
    }

    public abstract class Assertions<T, TAssertion> : ReferenceTypeAssertions<T, TAssertion>
        where TAssertion : ReferenceTypeAssertions<T, TAssertion>
    {
        private readonly Action<string> writeOutput;

        protected Assertions(T subject)
        {
            writeOutput = Assertions.WriteTestOutput;
            Subject = subject;
        }

        protected override string Context => typeof(T).Name;

        protected void CheckedThat(string message)
        {
            string traceMessage = $"Checked that {message}.";

            writeOutput?.Invoke(traceMessage);
        }

        protected AndConstraint<TAssertion> And()
        {
            return new AndConstraint<TAssertion>(this as TAssertion);
        }

        protected AndWhichConstraint<TAssertion, TWhich> AndWhich<TWhich>(TWhich newSubject)
        {
            return new AndWhichConstraint<TAssertion, TWhich>(this as TAssertion, newSubject);
        }
    }
}
