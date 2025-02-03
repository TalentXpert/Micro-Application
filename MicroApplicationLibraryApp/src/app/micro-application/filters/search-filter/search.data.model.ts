
export class SearchDataModel {
  
  fliterData(json: any[], args: string, property: string) {
    if (!json || !args) return json;
    let result: any[] = [];
    for (var i in json) {
      outerLoop:
      for (let key in json[i]) {
        let data = json[i][key];
        if (this.checkGuid(data)) continue;
        let dataType = typeof data;
        switch (dataType) {
          case 'string': if (data.toLowerCase().includes(args.trim().toLowerCase())) {
            result.push(json[i]);
            break outerLoop;
          }
          else break;

          case 'number': if (data.toString().includes(args.trim().toLowerCase())) {
            result.push(json[i]);
            break outerLoop;
          }
          else break;
          case 'object':
            let value = data[property];
            if (this.checkGuid(value)) continue;
            let dataType1 = typeof value;
            switch (dataType1) {
              case 'string': if (value.toLowerCase().includes(args.trim().toLowerCase())) {
                result.push(json[i]);
                break outerLoop;
              }
              else break;

              case 'number': if (value.toString().includes(args.trim().toLowerCase())) {
                result.push(json[i]);
                break outerLoop;
              }
              else break;
              case 'boolean': if (args == 'Open' || args == 'open' || args == 'Yes' || args == 'yes') {
                if (value.toString() == "true") {
                  result.push(json[i]);
                  break outerLoop;
                }
              }
              else if (args == 'Close' || args == 'close' || args == 'No' || args == 'no') {
                if (value.toString() == "false") {
                  result.push(json[i]);
                  break outerLoop;
                }
              }
              else break;
            }

        }
      }
    }
    return result;
  }

  checkGuid(guid: string) {
    var regexGuid = /^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$/gi;
    if (regexGuid.test(guid)) return true;
    return false;
  }
}
