export function Preservable() {
  return function (target: any) {
    target.prototype.preserveState = true;
  };
}
