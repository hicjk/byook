const codeOwnersMap = {
    scripts: ['배수진'],
    services: {
        'business-ledger': ['고찬균', '배수진'],
        'toss-card': ['채주민', '유재섭'],
        subsidy: ['허주엽'],
        payments: ['유재섭'],
        other: {
            test: ""
        }
    },
    libraries: {
        'yarn-workspaces-plugin-since': ['유재섭', '배수진'],
        tds: ['유병솔', '강두한']
    }
}

solution(codeOwnersMap, "services/business-ledger");

function solution(codeOwnersMap, directory) {
    if(directory.indexOf("/") < 0) {
        return codeOwnersMap[directory];
    }

    const split = directory.split("/");

    let result = codeOwnersMap;

    split.map(string => {
        result = result[string];

        console.log(result);
    });

    split.forEach(string => {

    });

    return result;
}

ddd.forEach()