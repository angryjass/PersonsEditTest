import Vue from 'vue'
import 'es6-promise/auto'
import App from '../src/App.vue'
import vuetify from '@/plugins/vuetify'
import store from './store'

new Vue({
  vuetify,
  store: store,
  render: h => h(App),
  }).$mount('#app')