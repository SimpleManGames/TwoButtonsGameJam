namespace HSM
{
    using System.Collections.Generic;
    using System.Reflection;

    public class StateMachineBuilder
    {
        private readonly State _root;

        private const BindingFlags MACHINE_FIELD_FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

        public StateMachineBuilder(State root)
        {
            _root = root;
        }

        public StateMachine Build()
        {
            StateMachine m = new StateMachine(_root);
            Wire(_root, null, m, new HashSet<State>());
            return m;
        }

        private static void Wire(State state, State parent, StateMachine machine, HashSet<State> visited)
        {
            if (state == null)
                return;

            if (!visited.Add(state))
                return;

            FieldInfo machineField = typeof(State).GetField(nameof(State.machine), MACHINE_FIELD_FLAGS);

            if (machineField != null)
                machineField.SetValue(state, machine);
            
            FieldInfo parentField = typeof(State).GetField(nameof(State.parent), MACHINE_FIELD_FLAGS);
            if (parentField != null)
                parentField.SetValue(state, parent);

            foreach (FieldInfo field in state.GetType().GetFields(MACHINE_FIELD_FLAGS))
            {
                if (!typeof(State).IsAssignableFrom(field.FieldType))
                    continue;

                if (field.Name == nameof(State.parent))
                    continue;

                State child = (State)field.GetValue(state);

                if (child == null)
                    continue;

                // if (!ReferenceEquals(child.parent, state))
                //     continue;

                Wire(child, state, machine, visited);
            }
        }
    }
}
