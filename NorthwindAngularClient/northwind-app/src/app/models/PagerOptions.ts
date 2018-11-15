export class PagerOptions {
  pageIndex: number;
  itemsPerPage: number;

  constructor(pageIndex, itemsPerPage = 50) {
      this.itemsPerPage = itemsPerPage;
      this.pageIndex = pageIndex;
  }

  static getDefaultOptions(): PagerOptions {
      return new PagerOptions(1);
  }
}
