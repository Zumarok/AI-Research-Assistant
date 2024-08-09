window.scrollToBottom = (element) => {
    element.scrollTop = element.scrollHeight;
};

function isBrowser() {
    return typeof window !== "undefined";
}