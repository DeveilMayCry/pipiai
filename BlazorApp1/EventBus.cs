namespace BlazorApp1
{
    public static class EventBus
    {
        private static Dictionary<string, object> _args = new Dictionary<string, object>();
        private static Dictionary<string, Action<object>> _actions = new Dictionary<string, Action<object>>();


        public static void Emit(string eventKey, object args)
        {
            var message = args as string;
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            _args[eventKey] = args;

            var action = _actions[eventKey];
            if (action != null)
            {
                action(message);
            }
        }

        public static void On(string eventKey, Action<object> action)
        {
            if(_actions.ContainsKey(eventKey))
            {
                _actions[eventKey] += action;
            }
            else
            {
                _actions[eventKey] = action;
            }

            if (!_args.ContainsKey(eventKey))
            {
                return;
            }

            var args = _args[eventKey];
            action(args);
        }
    }
}
