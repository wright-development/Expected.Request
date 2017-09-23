using Expected.Request.Tests.IntegrationTests;

namespace Expected.Request.Tests.Builders
{
    public class TodoModelBuilder
    {

        private bool _checked = false;
        private string _id = null;
        private string _text = "Foo Text";

        public TodoModelBuilder Checked(bool value)
        {
            _checked = value;
            return this;
        }

        public TodoModelBuilder WithId(string value)
        {
            _id = value;
            return this;
        }

        public TodoModelBuilder WithText(string value)
        {
            _text = value;
            return this;
        }

        public TodoModel Build()
        {
            return new TodoModel
            {
                Id = _id,
                Checked = _checked,
                Text = _text
            };
        }

        public static implicit operator TodoModel(TodoModelBuilder builder)
        {
            return builder.Build();
        }
    }
}
