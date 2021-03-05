const Fetch = (endPoint, data, type) => {
    if (type === 'GET') {
        return fetch(endPoint, {
            method: type,
            headers: {
                "Authorization": '4d684533-7d3f-4aaf-bcb4-4d13b6afd0f5'
            }
        })
            .then(json => {
                return json.json();
            })
    } else if (type === 'POST') {
        return fetch(endPoint, {
            method: type,
            headers: {
                "Authorization": '4d684533-7d3f-4aaf-bcb4-4d13b6afd0f5',
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })
            .then(json => {
                return json.json();
            })
    } else if (type === 'PUT') {
        return fetch(endPoint, {
            method: type,
            headers: {
                "Authorization": '4d684533-7d3f-4aaf-bcb4-4d13b6afd0f5',
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })
            .then(json => {
                return json.json();
            })
    } else if (type === 'PATCH') {
        return fetch(endPoint, {
            method: type,
            headers: {
                "Authorization": '4d684533-7d3f-4aaf-bcb4-4d13b6afd0f5'
            }
        })
            .then(json => {
                return json.json();
            })
    }
}