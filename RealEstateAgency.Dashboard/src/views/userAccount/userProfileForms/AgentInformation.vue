<template>
    <b-col sm="12">
    <b-card>
      <div slot="header">
        <strong>Edit Agent Information</strong>
      </div>
      <b-row>
        <b-col sm="5">
          <b-form-group>
            <label>Bussiness card front</label>
            <b-img class="pointToChange" src="https://picsum.photos/300/150/?image=41" fluid-grow alt="Fluid-grow image"></b-img>
          </b-form-group>
        </b-col>
        <b-col sm="2"></b-col>
        <b-col sm="5">
          <b-form-group>
            <label for="postal-code">Bussiness card back</label>
            <b-img class="pointToChange" src="https://picsum.photos/300/150/?image=41" fluid-grow alt="Fluid-grow image"></b-img>
          </b-form-group>
        </b-col>
        <b-col sm="12">
          <b-form-group>
            <label for="postal-code">Description</label>
            <quill-editor :options="editorOptions"></quill-editor>
          </b-form-group>
        </b-col>
      </b-row>
      <b-row>
        <submit-group v-on:onCancel="onCancel" v-on:onSubmit="onSubmit" />
      </b-row>
    </b-card>
  </b-col>
</template>

<script>
export default {
name: "AgentInformation",
  data() {
    return {
      editorOptions: {
          placeholder: "Enter description here ...",
          modules: {
            toolbar:  [
              ['bold', 'italic', 'underline', 'strike'],
              ['blockquote', 'code-block'],
              [{ 'header': 1 }, { 'header': 2 }],
              [{ 'list': 'ordered' }, { 'list': 'bullet' }],
              [{ 'script': 'sub' }, { 'script': 'super' }],
              [{ 'indent': '-1' }, { 'indent': '+1' }],
              [{ 'direction': 'rtl' }],
              [{ 'size': ['small', false, 'large', 'huge'] }],
              [{ 'header': [1, 2, 3, 4, 5, 6, false] }],
              [{ 'color': [] }],
              [{ 'align': [] }],
              ['clean'],
            ]
          }
        },
      selected: [], // Must be an array reference!
      show: true,
    };
  },
  methods: {
    onSubmit() {
      // do nothing
    },
    onCancel() {}
  }
}
</script>

<style>
.pointToChange {
    border-radius: 10px
}
.pointToChange:hover {
    cursor: pointer;
}
</style>
