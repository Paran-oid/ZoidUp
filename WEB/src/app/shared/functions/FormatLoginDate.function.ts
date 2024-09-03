export function FormatLoginDate(time: string) {
  const today: any = new Date();
  const previousDate: any = new Date(time);
  const diffTime = Math.abs(today - previousDate);

  let difference = Math.floor(diffTime / (1000 * 60 * 60 * 24));
  let result = Math.floor(difference).toString() + ' day(s) ago';
  if (difference == 0) {
    difference = Math.floor(diffTime / (1000 * 60 * 60));
    result = difference.toString() + ' hour(s) ago';
    if (difference == 0) {
      difference = Math.floor(diffTime / (1000 * 60));
      const diffMinutes = difference.toString() + ' minute(s) ago';

      if (difference == 0) {
        result = 'Online';
      }
    }
  }
  const final = result != 'Online' ? 'Offline:' + result : result;
  return final;
}
