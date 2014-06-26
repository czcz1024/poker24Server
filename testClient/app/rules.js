var rules = {};

rules.allow= function(arr) {
    return rules.isBoom(arr) || rules.isPair(arr) || rules.isSingle(arr) || rules.isSeq(arr);
}

rules.isBoom = function(arr) {
    return arr.length > 2 && rules.allSame(arr);
};

rules.isPair = function(arr) {
    return arr.length == 2 && rules.allSame(arr);
};

rules.isSingle = function (arr) { return arr.length == 1; };

rules.isSeq = function (arr) {
    for (var i = 0; i < arr.length; i++) {
        if (arr[i] >14 || arr[i]<1) {
            return false;
        }
    }
    var cha = [];
    var sort = arr.sort(function(a, b) { return a - b; });
    for (var i = 0; i < sort.length - 1; i++) {
        cha.push(sort[i + 1] - sort[i]);
    }
    return rules.allSame(cha);
};

rules.allSame = function(arr) {
    if (arr.length < 1) return false;
    var v1 = arr[0];
    for (var i = 1; i < arr.length; i++) {
        if (arr[i] != v1)return false;
    }
    return true;
};

rules.isBiger= function(last, arr) {
    if (rules.isBoom(last)) {
        if (rules.isBoom(arr)) {
            if (arr.length > last.length) return true;

            if (arr[0] > last[0]) return true;
        }
        if (rules.isPair(arr)) {
            if (arr[0] == 4) return true;
        }
    }

    if (rules.isPair(last)) {
        if (rules.isBoom(arr)) return true;
        if (rules.isPair(arr)) {
            if (arr[0] > last[0]) return true;
        }
    }
    if (rules.isSingle(last)) {
        if (rules.isBoom(arr)) return true;
        if (rules.isSingle(arr)) {
            if (arr[0] > last[0]) return true;
        }
    }
    if (rules.isSeq(last)) {
        if (rules.isBoom(arr)) return true;
        if (rules.isSeq(arr)) {
            if (arr.length == last.length) {
                if (arr[0] > last[0]) return true;
            }
        }
    }


    return false;
}



rules.count= function(arr, v) {
    var cnt = 0;
    for (var i = 0; i < arr.length; i++) {
        if (arr[i] == v)cnt++;
    }
    return cnt;
}

rules.may= function(arr) {
    

}


rules.except= function(arr, exp) {
    var r = [];
    for (var i = 0; i < arr.length; i++) {
        var no = false;
        for (var j = 0; j < exp.length; j++) {
            if (arr[i] == exp[j]) {
                no = true;
                break;
            }
        }
        if (!no) {
            r.push(arr[i]);
        }
    }
    return r;
}


rules.twoAs2= function(arr) {
    var r = [];
    for (var i = 0; i < arr.length; i++) {
        if (arr[i] == 15) {
            r.push(2);
        } else {
            r.push(arr[i]);
        }
    }
    return r;
}

