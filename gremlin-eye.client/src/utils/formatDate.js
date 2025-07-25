const gameDateFormatter = new Intl.DateTimeFormat("en-US", {
    dateStyle: "medium",
    timeZone: "UTC"
});

export default function formatString(dateTimeString) {
    if (dateTimeString == null)
        return 'TBA';
    
    if (dateTimeString instanceof Date)
        return gameDateFormatter.format(dateTimeString);

    return gameDateFormatter.format(new Date(dateTimeString.trim()));
};