export function FormatRequestDate(dateInput: string) {
  const today = new Date();
  const date = new Date(dateInput);
  const dateDiff = Math.abs(date.getTime() - today.getTime());

  const minutes = dateDiff / (1000 * 60);
  if (minutes < 60) {
    if (Math.floor(minutes) == 0) {
      return 'sent just now';
    }
    return `sent since ${Math.round(minutes)} minutes`;
  } else {
    let hours = dateDiff / (1000 * 60 * 60);
    const days = hours / 24;

    if (days > 1) {
      return `sent since ${Math.round(days)} days`;
    } else {
      return `sent since ${Math.round(hours)} hours`;
    }
  }
}
