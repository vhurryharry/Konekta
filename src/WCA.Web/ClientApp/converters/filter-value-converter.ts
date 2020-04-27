export class FilterValueConverter {
    public toView(array, searchTerm, filterFunction) {
      return array.filter((item) => {
        return searchTerm && searchTerm.length > 0 ? filterFunction(searchTerm, item) : true;
      });
    }
}
