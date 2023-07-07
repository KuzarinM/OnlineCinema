const APIHelper = {
    data(){
        return{
            _tokenPrefix : "Bearer ",
            _urlPrefix : `http://${'192.168.43.62'}/api`,
            _urlFilePrefix : `http://${'192.168.43.62'}:10101`
        }
    },
    methods:{
        async apiRequest(method, url){
            return await fetch(`${this._urlPrefix}${url}`,{
                method: method,
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                    "Authorization":`${this._tokenPrefix}${sessionStorage.getItem("token")}`
                    }
            })
        },
        async apiRequest(method, url, data){
            return await fetch(`${this._urlPrefix}${url}`,{
                method: method,
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                    "Authorization":`${this._tokenPrefix}${sessionStorage.getItem("token")}`
                    },
                body:JSON.stringify(data)
            })
        },
        async apiRequestText(method, url){
            const responce = await this.apiRequest(method, url)
            if(responce.status == 200){
                return await responce.text()
            }
            return null
        },
        async apiRequestJson(method, url){
            const responce = await this.apiRequest(method, url)
            if(responce.status == 200){
                return await responce.json()
            }
            return null
        },
        async apiRequestJson(method, url, data){
            const responce = await this.apiRequest(method, url, data)
            if(responce.status == 200){
                return await responce.json()
            }
            return null
        },
        logout(){
            if(confirm(`Вы действительно хотите выйти из под пользоватлея ${sessionStorage.getItem('user')}?`)){
                sessionStorage.removeItem("token")
                sessionStorage.removeItem("user")
                sessionStorage.removeItem("role")
                location = location
            }
        },
        getUserName(){
            return sessionStorage.getItem("user")
        },
        isAuthtorised(){
            return sessionStorage.getItem("token")!=null
        },
        isRole(role){
            return sessionStorage.getItem("role") == role
        }
    }

}
export default APIHelper;