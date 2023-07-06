<script>
	import $ from "jquery"; 
	import APIHelper from "../mixins/APIHelper.js";
	import { RouterLink} from 'vue-router'
    
	export default{
		props:{
			myObject : Object,
			fieldNames:{
				default:['name'],
				type:Array
			},
			priceField:{
				default:{
					fieldName:'price',
					saleFieldName:"newPrice",
				},
				type:Object
			},
			hasDiscount:{
				default:false,
				type:Boolean
			},
			openObjectUrl: {
				default:"/product/"
			},
			imageURL:{
				default:"http://localhost:5173/src/assets/icon.png",
				type:String
			}
		},
		data(){
			return{
				urlI:""
			}
		},
		computed:{
			cssVars(){
				return{
					'--image-url' : 'url("'+this.urlI+'")',
				}
			}
		},
		methods:{
			open(){
				if(this.openObjectUrl!=null){
					this.$router.push(`${this.openObjectUrl}${this.myObject.id}`)
				}
				else{
					this.$emit("click")
				}
			}
		},
		async mounted(){
			this.urlI= await this.imageURL
		}
	}
</script>

<template>
	<a class="nav-link justify-content-bottom product d-flex" :style="cssVars" :href="`${this.openObjectUrl}${this.myObject.id}`">
		<div class="img-product"></div>
		<h4 class="text-wrap text-center" v-for="item in this.fieldNames">{{ myObject[item].replaceAll("_", " ").replaceAll(".", " ") }}</h4>
	</a>
</template>

<style scoped>
    .product{
		display: flex;
		flex-direction: column;
        border-radius: 8px;
        border-color: #000000;
        background-color: #bbf5ba!important;
        border-style: solid;
        border-width: 3px;
        justify-content: end;
        align-items: center;
        margin: 5px 5px 5px 5px;
		width: 300px;
		height: 400px;
    }
	.img-product{
        background-repeat: no-repeat;
        background-size: contain;
		background-position: center;
		background-image: var(--image-url);
		width: 100%;
		height: 100%;
		margin: 0;
		padding: 0;
	}
</style>
