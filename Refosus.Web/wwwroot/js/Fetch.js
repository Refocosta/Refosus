const Fetch = (endPoint, data, type, user) => {
    const server = 'https://nativacrm.api.local/api/v1';
    if (type === 'GET') {
        return fetch(server + endPoint, {
            method: type,
            headers: {
                "Authorization": '4d684533-7d3f-4aaf-bcb4-4d13b6afd0f5',
                "User"         : user
            }
        })
            .then(json => {
                return json.json();
            })
    } else if (type === 'POST') {
        return fetch(server + endPoint, {
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
        return fetch(server + endPoint, {
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
        return fetch(server + endPoint, {
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

const FetchAsync = async (endPoint, data, type) => {
    return await fetch(endPoint, {
        method: type,
        headers: {
            "Authorization": '4d684533-7d3f-4aaf-bcb4-4d13b6afd0f5'
        }
    })
        .then(json => {
            return json.json();
        })
}