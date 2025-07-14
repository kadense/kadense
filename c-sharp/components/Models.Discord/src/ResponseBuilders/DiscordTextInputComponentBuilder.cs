namespace Kadense.Models.Discord.ResponseBuilders;

public class DiscordTextInputComponentBuilder<TParent> : DiscordComponentBuilder<TParent, DiscordTextInputComponent>
{
    public DiscordTextInputComponentBuilder(TParent parent, DiscordTextInputComponent component) : base(parent, component)
    {
    }

    public DiscordTextInputComponentBuilder<TParent> WithCustomId(string customId)
    {
        Component.CustomId = customId;
        return this;
    }

    public DiscordTextInputComponentBuilder<TParent> WithLabel(string label)
    {
        Component.Label = label;
        return this;
    }

    public DiscordTextInputComponentBuilder<TParent> WithStyle(int style)
    {
        Component.Style = style;
        return this;
    }

    public DiscordTextInputComponentBuilder<TParent> WithValue(string value)
    {
        Component.Value = value;
        return this;
    }

    public DiscordTextInputComponentBuilder<TParent> WithPlaceholder(string placeholder)
    {
        Component.Placeholder = placeholder;
        return this;
    }

    public DiscordTextInputComponentBuilder<TParent> WithMinLength(int minLength)
    {
        Component.MinLength = minLength;
        return this;
    }

    public DiscordTextInputComponentBuilder<TParent> WithMaxLength(int maxLength)
    {
        Component.MaxLength = maxLength;
        return this;
    }

    public DiscordTextInputComponentBuilder<TParent> WithRequired(bool required)
    {
        Component.Required = required;
        return this;
    }
}