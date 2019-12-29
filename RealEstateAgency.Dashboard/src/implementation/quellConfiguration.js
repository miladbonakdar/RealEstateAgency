import Vue from "vue";
import VueQuillEditor from 'vue-quill-editor'
import 'quill/dist/quill.core.css'
import 'quill/dist/quill.snow.css'
import 'quill/dist/quill.bubble.css'

export default function () {
//https://github.com/surmon-china/vue-quill-editor
  Vue.use(VueQuillEditor);
}
