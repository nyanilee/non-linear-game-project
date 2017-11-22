namespace Dialogue {
    using System.Collections.Generic;

    using Yarn;

    public class DefaultVariableStorage : VariableStorage {
        private Dictionary<string, Value> variables =
            new Dictionary<string, Value>();

        public void Clear() {
        }

        public float GetNumber(string variableName) {
            return this.variables[variableName].AsNumber;
        }

        public Value GetValue(string variableName) {
            return this.variables[variableName];
        }

        public void SetNumber(string variableName, float number) {
            this.variables[variableName] = new Value(number);
        }

        public void SetValue(string variableName, Value value) {
            this.variables[variableName] = new Value(value);
        }
    }
}